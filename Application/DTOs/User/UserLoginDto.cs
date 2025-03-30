﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.User
{
    public class UserLoginDto
    {
        public string? UserNameOrEmail { get; set; }

        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
