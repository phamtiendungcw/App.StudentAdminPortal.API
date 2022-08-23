﻿using DataModels = App.StudentAdminPortalAPI.DataModels;
using App.StudentAdminPortalAPI.DomainModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace App.StudentAdminPortalAPI.Profiles.AfterMaps
{
    public class UpdateStudentRequestAfterMap : IMappingAction<UpdateStudentRequest, DataModels.Student>
    {
        public void Process(UpdateStudentRequest source, DataModels.Student destination, ResolutionContext context)
        {
            destination.Address = new DataModels.Address()
            {
                PhysicalAddress = source.PhysicalAddress,
                PostalAddress = source.PostalAddress
            };
        }
    }
}