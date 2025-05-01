namespace RampRage_WebAPI_ITStep.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddCorsPolicies(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "front-end-cors-policy",
                                  policy =>
                                  {
                                      policy.AllowAnyMethod();
                                      policy.AllowAnyHeader();
                                      policy.AllowAnyOrigin();
                                  });
            });
        }
    }
}
