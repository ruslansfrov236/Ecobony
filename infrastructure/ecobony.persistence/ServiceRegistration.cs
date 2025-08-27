namespace ecobony.persistence;

public static class ServiceRegistration
{
    public static void AddPersistenceRegistration(this IServiceCollection service)
    {
        service.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(Configuration.ConnectionString));


        service.AddIdentity<AppUser, AppRole>(options =>
            {

                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;


                options.User.RequireUniqueEmail = true;


                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;


                options.SignIn.RequireConfirmedEmail = true;

            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        #region Repository

        service
            .AddScoped<IWasteSortingMistakeTranslationReadRepository, WasteSortingMistakeTranslationReadRepository>();
        service
            .AddScoped<IWasteSortingMistakeTranslationWriteRepository, WasteSortingMistakeTranslationWriteRepository>();

        service.AddScoped<ISortingActionTranslationReadRepository, SortingActionTranslationReadRepository>();
        service.AddScoped<ISortingActionTranslationWriteRepository, SortingActionTranslationWriteRepository>();

        service.AddScoped<IWasteSortingMistakeReadRepository, WasteSortingMistakeReadRepository>();
        service.AddScoped<IWasteSortingMistakeWriteRepository, WasteSortingMistakeWriteRepository>();

        service.AddScoped<ICategoryTranslationReadRepository, CategoryTranslationReadRepository>();
        service.AddScoped<ICategoryTranslationWriteRepository, CategoryTranslationWriteRepository>();

        service.AddScoped<IHeaderTranslationReadRepository, HeaderTranslationReadRepository>();
        service.AddScoped<IHeaderTranslationWriteRepository, HeaderTranslationWriteRepository>();

        service.AddScoped<ISortingActionReadRepository, SortingActionReadRepository>();
        service.AddScoped<ISortingActionWriteRepository, SortingActionWriteRepository>();

        service.AddScoped<IBonusComunityReadRepository, BonusComunityReadRepository>();
        service.AddScoped<IUserTrackingReadRepository, UserTrackingReadRepository>();
        service.AddScoped<IUserTrackingWriteRepository, UserTrackingWriteRepository>();
        service.AddScoped<IBonusComunityWriteRepository, BonusComunityWriteRepository>();

        service.AddScoped<IWasteProcessReadRepository, WasteProcessReadRepository>();
        service.AddScoped<IWasteProcessWriteRepository, WasteProcessWriteRepository>();

        service.AddScoped<IBalanceTransferReadRepository, BalanceTransferReadRepository>();
        service.AddScoped<IBalanceTransferWriteRepository, BalanceTransferWriteRepository>();
        service.AddScoped<IUserHistoryReadRepository, UserHistoryReadRepository>();
        service.AddScoped<IUserHistoryWriteRepository, UserHistoryWriteRepository>();
        service.AddScoped<IBalanceReadRepository, BalanceReadRepository>();
        service.AddScoped<IBalanceWriteRepository, BalanceWriteRepository>();

        service.AddScoped<ILocationReadRepository, LocationReadRepository>();
        service.AddScoped<ILocationWriteRepository, LocationWriteRepository>();

        service.AddScoped<ILanguageReadRepository, LanguageReadRepository>();
        service.AddScoped<ILanguageWriteRepository, LanguageWriteRepository>();

        service.AddScoped<ICategoryReadRepository, CategoryReadRepository>();
        service.AddScoped<ICategoryWriteRepository, CategoryWriteRepository>();

        service.AddScoped<IHeaderReadRepository, HeaderReadRepository>();
        service.AddScoped<IHeaderWriteRepository, HeaderWriteRepository>();

        service.AddScoped<IBonusReadRepository, BonusReadRepository>();
        service.AddScoped<IBonusWriteRepository, BonusWriteRepository>();
        service.AddScoped<IWasteReadRepository, WasteReadRepository>();
        service.AddScoped<IWasteWriteRepository, WasteWriteRepository>();

        #endregion


        #region Service


        service.AddTransient<IUserHistoryService, UserHistoryService>();
        service.AddScoped<ICategoryService, CategoryService>();
        service.AddScoped<IHeaderService, HeaderService>();
        service.AddScoped<IWasteService, WasteService>();
        service.AddScoped<IBonusService, BonusService>();
        service.AddScoped<IAuthService, AuthService>();
        service.AddScoped<IUserService, UserService>();
        service.AddScoped<ILanguageService, LanguageService>();
        service.AddScoped<IRoleService, RoleService>();
        service.AddScoped<IBalanceService, BalanceService>();
        #endregion

    }
}
