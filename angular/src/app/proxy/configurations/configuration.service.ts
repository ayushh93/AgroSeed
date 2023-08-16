import type { ConfigurationDto, CreateUpdateConfigurationDto } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ConfigurationService {
  apiName = 'Default';
  

  create = (input: CreateUpdateConfigurationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConfigurationDto>({
      method: 'POST',
      url: '/api/app/configuration',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/configuration/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConfigurationDto>({
      method: 'GET',
      url: `/api/app/configuration/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<ConfigurationDto>>({
      method: 'GET',
      url: '/api/app/configuration',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateConfigurationDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, ConfigurationDto>({
      method: 'PUT',
      url: `/api/app/configuration/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
