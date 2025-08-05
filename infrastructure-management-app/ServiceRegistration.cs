﻿using FluentValidation.AspNetCore;

using Application.Validators.Department;
using Application.Validators.Equipment;
using Application.Validators.UsageHistory;
using Application.Validators.User;
//using Microsoft.AspNetCore.Hosting;
//using FluentValidation.AspNetCore;

namespace infrastructure_management_app
{
    public static class ServiceRegistration
    {

        public static void AddValidationService(this IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(option =>
            {
                option.RegisterValidatorsFromAssemblyContaining<EquipmenCreatetValidator>();
                option.RegisterValidatorsFromAssemblyContaining<DepartmentCreateValidator>();
                option.RegisterValidatorsFromAssemblyContaining<UserLoginDtoValidator>();
                //option.RegisterValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
                option.RegisterValidatorsFromAssemblyContaining<UsageHistoryCreateValidator>();
            });


            //string imageSavePath = Path.Combine(_webHostEnvironment.ContentRootPath, "images");
            //services.AddSingleton<IWebHostEnvironment>(_ => _env);
            //services.AddSingleton(new ImageSaveConfig(imageSavePath));
        }
    }
}
