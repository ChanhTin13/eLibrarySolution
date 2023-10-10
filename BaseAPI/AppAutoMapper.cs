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
            // Thể loại
            CreateMap<CategoryCreate, tbl_Category>().ReverseMap();
            CreateMap<CategoryUpdate, tbl_Category>().ReverseMap(); 
            // tác giả
            CreateMap<AuthorCreate, tbl_Author>().ReverseMap();
            CreateMap<AuthorUpdate, tbl_Author>().ReverseMap(); 
            // đầu sách
            CreateMap<TitleCreate, tbl_Title>().ReverseMap();
            CreateMap<TitleUpdate, tbl_Title>().ReverseMap(); 
            // sách
            CreateMap<BookCreate, tbl_Book>().ReverseMap();
            CreateMap<BookUpdate, tbl_Book>().ReverseMap(); 
        }

    }
}
