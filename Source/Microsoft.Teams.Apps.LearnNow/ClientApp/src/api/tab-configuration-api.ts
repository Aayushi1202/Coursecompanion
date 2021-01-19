// <copyright file="tab-configuration-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axios from "./axios-decorator";
import { ITabConfiguration } from "../model/type";
import { AxiosResponse } from "axios";

/**
* Save tab configuration details in the storage.
* @param tabConfigurationDetail {ITabConfiguration} tab configuration object to be stored in database.
*/
export const createTabConfiguration = async (tabConfigurationDetail: ITabConfiguration): Promise<AxiosResponse<ITabConfiguration>> => {
    let url = '/api/tab-configuration';
    return await axios.post(url, tabConfigurationDetail);
}

/**
* update tab configuration details in the storage.
* @param tabConfigurationDetail {ITabConfiguration} tab configuration object to be updated in database.
* @param tabId {String} Unique tab identifier.
*/
export const updateTabConfiguration = async (tabConfigurationDetail: ITabConfiguration, tabId: string): Promise<AxiosResponse<ITabConfiguration>> => {
    let url = `/api/tab-configuration/${tabId}`;
    return await axios.patch(url, tabConfigurationDetail);
}

/**
* Get tab configuration details for given tab id.
* @param tabId {String} tab id of the teams tab for which tab configuration detail need to obtained.
*/
export const getTabConfiguration = async (tabId: string): Promise<any> => {
    let url =`/api/tab-configuration/${tabId}`;
    return await axios.get(url);
}