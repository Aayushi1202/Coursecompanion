// <copyright file="learning-module-items.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Button, Flex, Input, Loader } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";
import { SearchIcon } from "@fluentui/react-icons-northstar";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import LearningModuleTable from "./learning-module-table";
import { createResourceModuleMapping, getLearningModules } from "../../api/learning-module-api";
import { IFilterModel, IResourceModuleDetails } from "../../model/type";

import "../../styles/learning-module.css";
import Resources from "../../constants/resources";

interface ILearningModuleItemData {
    id: string;
    grade?: any;
    imageUrl: string;
    subject?: any;
    title: string;
    description: string;
    checkItem?: boolean;
}

interface ILearningModuleItemState {
    isLoading: boolean;
    userSelectedItem: string;
    filteredItem: ILearningModuleItemData[];
    learningModuleData: ILearningModuleItemData[];
    searchValue: string;
    isSubmitLoading: boolean;
    screenWidth: number;
}

/**
 * Component to render learning module collection in add to learning module page.
 */
class LearningModuleItems extends React.Component<
    WithTranslation,
    ILearningModuleItemState
    > {
    localize: TFunction;
    history: any;
    gradeId: string | null = null;
    subjectId: string | null = null;
    resourceId: string | null = null;

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            isLoading: true,
            userSelectedItem: "",
            searchValue: "",
            filteredItem: [],
            learningModuleData: [],
            isSubmitLoading: false,
            screenWidth: window.innerWidth,
        };
        this.history = props.history;
        let search = this.history.location.search;
        let params = new URLSearchParams(search);
        this.gradeId = params.get("gradeId") ? params.get("gradeId") : "";
        this.subjectId = params.get("subjectId") ? params.get("subjectId") : "";
        this.resourceId = params.get("resourceId") ? params.get("resourceId") : "";
        this.handleKeyPress = this.handleKeyPress.bind(this);
    }

    /**
     * Used to initialize Microsoft Teams sdk
     */
    public async componentDidMount() {
        this.setState({ isLoading: true });
        this.getAllLearningModules();
        window.addEventListener("resize", this.update);
    }

    public componentDidUnmount() {
        window.removeEventListener('resize', this.update);
    }

    /**
    * Get screen width real time
    */
    update = () => {
        if (window.innerWidth !== this.state.screenWidth) {
            this.setState({ screenWidth: window.innerWidth });
        }
    };

    /**
     * Fetch posts for user private list tab from API
     */
    private getAllLearningModules = async () => {
        var filterRequestDetails: IFilterModel = {
            subjectIds: [this.subjectId!],
            gradeIds: [this.gradeId!],
            tagIds: [],
            createdByObjectIds: [],
        };

        let response = await getLearningModules(0, filterRequestDetails);
        if (response.status === 200 && response.data) {
            // bind all learning modules data
            this.setState({
                filteredItem: response.data,
                learningModuleData: response.data,
                isLoading: false,
            });
        } else {
            this.setState({
                isLoading: false,
            });
        }
    };

    /**
     * Set State value of text box input control
     * @param  {Any} event Event object
     */
    private handleChange = (event: any) => {
        this.setState({ searchValue: event.target.value });
        this.handleLearningModuleSearch(event.target.value);
    };

    /**
     * Used to call parent search method on enter key press in text box
     * @param  {Any} event Event object
     */
    private handleKeyPress = (event: any) => {
        var keyCode = event.which || event.keyCode;
        if (keyCode === Resources.keyCodeEnter) {
            if (event.target.value.length > 2 || event.target.value === "") {
                this.handleLearningModuleSearch(event.target.value);
            }
        }
    };

    /**
     *Submits and adds new learning module resource.
     */
    private onAddButtonClick = async () => {
        if (this.checkIfSubmitAllowed()) {
            this.setState({ isSubmitLoading: true });
            var resourceModuleData: IResourceModuleDetails = {
                ResourceId: this.resourceId!,
                LearningModuleId: this.state.userSelectedItem,
            };

            let response = await createResourceModuleMapping(
                resourceModuleData
            );
            if (response.status === 200 || response.status === 409) {
                let details: any = { isSuccess: true };
                microsoftTeams.tasks.submitTask(details);
            }
            this.setState({ isSubmitLoading: true });
        }
    };

    /**
     *Checks whether all validation conditions are matched before user submits new grade request
     */
    private checkIfSubmitAllowed = () => {
        return this.state.userSelectedItem.length > 0;

    };

    /**
    *Filters table as per search text entered by user
    *@param {String} searchText Search text entered by user
    */
    private handleLearningModuleSearch = (searchText: string) => {
        if (searchText) {
            var filteredLearningModule = this.state.learningModuleData.filter(function (learningModule) {
                return learningModule.title.toUpperCase().includes(searchText.toUpperCase());
            });
            this.setState({ filteredItem: filteredLearningModule });
        }
        else {
            this.setState({ filteredItem: this.state.learningModuleData });
        }
    }


    /**
     * Navigate to add new module page
     */
    private handleCreateNewButtonClick = () => {
        this.history.push(
            `/createmodule?addresource=${true}&resourceId=${this.resourceId}`
        );
    };

    /**
     * Update selected learning modules data
     * @param {String} moduleId Learning module id
     * @param {Boolean} isSelected Represents whether module is selected or not
     */
    private onLearningModuleSelected = (moduleId: string, isSelected: boolean) => {
        let filteredItems: ILearningModuleItemData[] = [];
        if (isSelected) {
            let userSelectedModules = this.state.userSelectedItem;
            userSelectedModules = moduleId;

            this.state.filteredItem!.map((resource: ILearningModuleItemData) => {
                if (resource.id === moduleId) {
                    resource.checkItem = true;
                } else {
                    resource.checkItem = false;
                }
                filteredItems.push(resource);
            });
            this.setState({
                userSelectedItem: userSelectedModules,
                filteredItem: filteredItems,
            });
        } else {
            this.state.filteredItem!.map((resource: ILearningModuleItemData) => {
                if (resource.id === moduleId) {
                    resource.checkItem = false;
                }
                filteredItems.push(resource);
            });

            this.setState({
                userSelectedItem: "",
                filteredItem: filteredItems,
            });
        }
    };

    /**
     * Renders the component
     */
    public render(): JSX.Element {

        if (this.state.isLoading) {
            return (
                <div className="container-div">
                    <div className="container-subdiv">
                        <div className="loader">
                            <Loader />
                        </div>
                    </div>
                </div>
            );
        } else {
            return (
                <div>
                    <div className="add-lm-module-container">
                        <div className="search-container-add-lm">
                            <Flex gap="gap.small">
                                <Flex.Item>
                                    <Flex className="add-lm-search">
                                        <Input
                                            fluid
                                            icon={
                                                <SearchIcon
                                                    onClick={this.handleChange}
                                                />
                                            }
                                            placeholder={this.localize("searchModulePlaceHolder")}
                                            value={this.state.searchValue}
                                            onChange={this.handleChange}
                                            onKeyUp={this.handleKeyPress}
                                        />
                                    </Flex>
                                </Flex.Item>
                                <Flex.Item>
                                    <Flex column gap="gap.small" vAlign="start">
                                        <Button
                                            className="create-new-button"
                                            content={this.localize(
                                                "createNewLearningModuleButtonText"
                                            )}
                                            onClick={() => {
                                                this.handleCreateNewButtonClick();
                                            }}
                                            disabled={this.state.userSelectedItem.length > 0}
                                        />
                                    </Flex>
                                </Flex.Item>
                            </Flex>
                        </div>
                        <div className="learning-module-table-container">
                            {this.state.filteredItem.length > 0 ?
                                <LearningModuleTable
                                    showCheckbox={true}
                                    responsesData={this.state.filteredItem}
                                    onCheckBoxChecked={this.onLearningModuleSelected}
                                    screenWidth={this.state.screenWidth}
                                />
                                :
                                <div className="resource-validation">{this.localize("noModuleForResource")}</div>
                            }
                        </div>
                    </div>
                    <div className="add-form-button">
                        <Button
                            content={this.localize("doneButtonText")}
                            primary
                            loading={this.state.isSubmitLoading}
                            disabled={
                                this.state.isSubmitLoading ||
                                this.state.userSelectedItem.length <= 0
                            }
                            onClick={this.onAddButtonClick}
                        />
                    </div>
                </div>
            );
        }
    }
}

export default withTranslation()(LearningModuleItems);
