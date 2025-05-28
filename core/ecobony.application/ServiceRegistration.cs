
using ecobony.application.Validator;
using FluentValidation.AspNetCore;

namespace ecobony.application;

public static  class ServiceRegistration
{
  
    public static void AddApplicationRegistration(this IServiceCollection service)
    {
     
       
        service.AddMediatR(typeof(ServiceRegistration));
        service.AddHttpClient();
    }
}