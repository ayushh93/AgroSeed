import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  { path: 'configurations', loadChildren: () => import('./configuration/configuration.module').then(m => m.ConfigurationModule) },
  { path: 'customers', loadChildren: () => import('./customer/customer.module').then(m => m.CustomerModule) },
  { path: 'categories', loadChildren: () => import('./category/category.module').then(m => m.CategoryModule) },
  { path: 'brands', loadChildren: () => import('./brand/brand.module').then(m => m.BrandModule) },
  { path: 'company-infos', loadChildren: () => import('./company-info/company-info.module').then(m => m.CompanyInfoModule) },
  { path: 'products', loadChildren: () => import('./product/product.module').then(m => m.ProductModule) },
  { path: 'cycle-counts', loadChildren: () => import('./cycle-count/cycle-count.module').then(m => m.CycleCountModule) },
  { path: 'fiscal-years', loadChildren: () => import('./fiscal-year/fiscal-year.module').then(m => m.FiscalYearModule) },
  { path: 'opening-balances', loadChildren: () => import('./opening-balance/opening-balance.module').then(m => m.OpeningBalanceModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {})],
  exports: [RouterModule],
})
export class AppRoutingModule {}
