﻿using Entities;
using Entities.Search;
using Interface.Services.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Services
{
    public interface ILibraryLoanService:IDomainService<tbl_LibraryLoan,LibraryLoanSearch>
    {

    }
}
