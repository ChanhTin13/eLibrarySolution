﻿using Entities;
using Entities.DomainEntities;
using Entities.Search;
using Extensions;
using Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("api/Address")]
    [ApiController]
    [Description("Địa chỉ")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        protected ICitiesService citiesService;
        protected IDistrictsService districtsService;
        protected IWardsService wardsService;
        public AddressController(
            ICitiesService citiesService,
            IDistrictsService districtsService,
            IWardsService wardsService)
        {
            this.districtsService = districtsService;
            this.citiesService = citiesService;
            this.wardsService = wardsService;
        }
        /// <summary>
        /// Lấy danh sách tỉnh/thành phố
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("cities")]
        [AppAuthorize]
        [Description("Lấy sanh sách Tỉnh/TP")]
        public async Task<AppDomainResult> GetCities([FromQuery] BaseSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            if (ModelState.IsValid)
            {
                PagedList<tbl_Cities> pagedData = await citiesService.GetPagedListData(baseSearch);
                var citiesModels = (from i in pagedData.items
                                   select new tbl_Cities
                                   {
                                       id = i.id,
                                       name =  i.name,
                                       code = i.code,
                                       description = i.description,
                                       active = i.active,
                                       created = i.created,
                                       createdBy = i.createdBy,
                                       updated = i.updated,
                                       updatedBy = i.updatedBy,
                                   }).ToList();
                PagedList<tbl_Cities> pagedDataModel = new PagedList<tbl_Cities>
                {
                    pageIndex = pagedData.pageIndex,
                    pageSize = pagedData.pageSize,
                    totalItem = pagedData.totalItem,
                    items = citiesModels
                };
                appDomainResult = new AppDomainResult
                {
                    data = pagedDataModel,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sách quận/huyện
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("districts")]
        [AppAuthorize]
        [Description("Lấy danh sách Quận/Huyện")]
        public async Task<AppDomainResult> GetDistricts([FromQuery] DistrictsSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();

            if (ModelState.IsValid)
            {
                PagedList<tbl_Districts> pagedData = await districtsService.GetPagedListData(baseSearch);
                var districtsModels = (from i in pagedData.items
                                    select new tbl_Districts
                                    {
                                        id = i.id,
                                        name = i.name,
                                        code = i.code,
                                        description = i.description,
                                        cityId = i.cityId,
                                        cityName = i.cityName,
                                        active = i.active,
                                        created = i.created,
                                        createdBy = i.createdBy,
                                        updated = i.updated,
                                        updatedBy = i.updatedBy,
                                    }).ToList();
                PagedList<tbl_Districts> pagedDataModel = new PagedList<tbl_Districts>
                {
                    pageIndex = pagedData.pageIndex,
                    pageSize = pagedData.pageSize,
                    totalItem = pagedData.totalItem,
                    items = districtsModels
                };
                appDomainResult = new AppDomainResult
                {
                    data = pagedDataModel,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
        /// <summary>
        /// Lấy danh sách phường/xã
        /// </summary>
        /// <param name="baseSearch"></param>
        /// <returns></returns>
        [HttpGet("wards")]
        [AppAuthorize]
        [Description("Lấy danh sách phường xã")]
        public async Task<AppDomainResult> GetWards([FromQuery] WardsSearch baseSearch)
        {
            AppDomainResult appDomainResult = new AppDomainResult();
            if (ModelState.IsValid)
            {
                PagedList<tbl_Wards> pagedData = await wardsService.GetPagedListData(baseSearch);
                var districtsModels = (from i in pagedData.items
                                       select new tbl_Wards
                                       {
                                           id = i.id,
                                           name = i.name,
                                           code = i.code,
                                           description = i.description,
                                           districtId = i.districtId,
                                           districtName = i.districtName,
                                           active = i.active,
                                           created = i.created,
                                           createdBy = i.createdBy,
                                           updated = i.updated,
                                           updatedBy = i.updatedBy,
                                       }).ToList();
                PagedList<tbl_Wards> pagedDataModel = new PagedList<tbl_Wards>
                {
                    pageIndex = pagedData.pageIndex,
                    pageSize = pagedData.pageSize,
                    totalItem = pagedData.totalItem,
                    items = districtsModels
                };
                appDomainResult = new AppDomainResult
                {
                    data = pagedDataModel,
                    success = true,
                    resultCode = (int)HttpStatusCode.OK
                };
            }
            else
                throw new AppException(ModelState.GetErrorMessage());

            return appDomainResult;
        }
    }
}
