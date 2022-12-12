namespace CatalogoMinAPI.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        // Tratamento de exceção
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        // Habilitando CORS
        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {
            app.UseCors(p =>
            {
                p.AllowAnyOrigin();
                p.WithMethods("GET");
                p.AllowAnyMethod();
            });
            return app;
        }

        // Habilitando Middleware do Swagger
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { });

            return app;
        }
    }
}
