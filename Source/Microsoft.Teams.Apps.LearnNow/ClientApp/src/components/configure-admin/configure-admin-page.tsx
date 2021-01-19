// <copyright file="configure-admin-page.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Menu, tabListBehavior } from "@fluentui/react-northstar";
import GradeTabPage from "./grade-tab";
import SubjectTabPage from "./subject-tab";
import TagsTabPage from "./tag-tab";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";

import "../../styles/admin-configure-wrapper-page.css";

interface IConfigAdminState {
    activeIndex: number;
}

/**
* Parent Component for admin pages.
*/
class ConfigureAdminPage extends React.Component<WithTranslation, IConfigAdminState> {
    localize: TFunction;
    history: any;

    constructor(props) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            activeIndex: 0,
        }

        this.history = props.history;
    }

    /**
    * Renders the component
    * @param {Any} event Object which describes event occurred
    * @param {Any} menuItemDetail menu property details
    */
    private onMenuItemClick = (event: any, menuItemDetail: any) => {
        this.setState({
            activeIndex: menuItemDetail.activeIndex
        })
    }

    /**
    * Renders the component
    */
    public render(): JSX.Element {
        const menuItems = [
            {
                key: 'Grade',
                content: this.localize("gradeLabel"),
            },
            {
                key: 'Subject',
                content: this.localize("subjectLabel"),
            },
            {
                key: 'Tag',
                content: this.localize("tagLabel"),
            }
        ]

        return (
            <div>
                <div>
                    <div className="container-ui">
                        <Menu
                            className="admin-menu"
                            defaultActiveIndex={0}
                            items={menuItems}
                            onActiveIndexChange={(e: any, props: any) => this.onMenuItemClick(e, props)}
                            underlined
                            primary
                            accessibility={tabListBehavior}
                        />
                        {
                            this.state.activeIndex === 0 ? <GradeTabPage /> : this.state.activeIndex === 1 ? <SubjectTabPage /> : <TagsTabPage />
                        }
                    </div>
                </div>
            </div>
        )
    }
}

export default withTranslation()(ConfigureAdminPage);