using Entities;
using Models;
using Request;
using Utilities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Request.DomainRequests;
using Request.RequestCreate;
using Request.RequestUpdate;
using Entities.Search;

namespace Models.AutoMapper
{
    public class AppAutoMapper : Profile
    {
        public AppAutoMapper()
        {

            //người dùng
            CreateMap<UserCreate, tbl_Users>().ReverseMap();
            CreateMap<UserUpdate, tbl_Users>().ReverseMap();
            CreateMap<UserSearch, StudentSearchPrivate>().ReverseMap();
            CreateMap<UserSearch, StaffSearchPrivate>().ReverseMap();
            // tác giả
            CreateMap<AuthorCreate, tbl_Authors>().ReverseMap();
            CreateMap<AuthorUpdate, tbl_Authors>().ReverseMap();
            CreateMap<AuthorSearch, AuthorSearch>().ReverseMap();
            // đầu sách
            CreateMap<TitleCreate, tbl_Titles>().ReverseMap();
            CreateMap<TitleUpdate, tbl_Titles>().ReverseMap();
            CreateMap<TitleSearch, TitleSearch>().ReverseMap();
        }

    }
}
