using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class StockUser : IdentityUser
{
    public string StockUserName { get; set; }

}

