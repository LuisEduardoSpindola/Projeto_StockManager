using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class StockUser : IdentityUser
{
    [Display(Name = "Nome de usuário")]
    public string StockUserName { get; set; }

}

