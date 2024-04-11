using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

// Add profile data for application users by adding properties to the StockUser class
public class StockUser : IdentityUser
{
    public string Nome { get; set; }

    public int CodAcesso { get; set; }
}

