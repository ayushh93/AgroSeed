import type { CategoryDto, CreateUpdateCategoryDto, GetCategoryFilter } from './models';
import { RestService, Rest } from '@abp/ng.core';
import type { PagedAndSortedResultRequestDto, PagedResultDto } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import type { DropdownDto } from '../dtos/models';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  apiName = 'Default';
  

  create = (input: CreateUpdateCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CategoryDto>({
      method: 'POST',
      url: '/api/app/category',
      body: input,
    },
    { apiName: this.apiName,...config });
  

  delete = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, void>({
      method: 'DELETE',
      url: `/api/app/category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  get = (id: string, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CategoryDto>({
      method: 'GET',
      url: `/api/app/category/${id}`,
    },
    { apiName: this.apiName,...config });
  

  getCategories = (filter: GetCategoryFilter, config?: Partial<Rest.Config>) =>
    this.restService.request<any, DropdownDto[]>({
      method: 'GET',
      url: '/api/app/category/categories',
      params: { id: filter.id },
    },
    { apiName: this.apiName,...config });
  

  getList = (input: PagedAndSortedResultRequestDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, PagedResultDto<CategoryDto>>({
      method: 'GET',
      url: '/api/app/category',
      params: { sorting: input.sorting, skipCount: input.skipCount, maxResultCount: input.maxResultCount },
    },
    { apiName: this.apiName,...config });
  

  update = (id: string, input: CreateUpdateCategoryDto, config?: Partial<Rest.Config>) =>
    this.restService.request<any, CategoryDto>({
      method: 'PUT',
      url: `/api/app/category/${id}`,
      body: input,
    },
    { apiName: this.apiName,...config });

  constructor(private restService: RestService) {}
}
