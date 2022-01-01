
namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Provides extension methods to create Composite Validator with different settings.
    /// </summary>
    public static class ValidatorBuilderExtensions
    {
        /// <summary>
        /// Creates composite validator with default validation settings.
        /// </summary>
        /// <param name="validatorBuilder">Validator Builder.</param>
        /// <returns>Composite validator.</returns>
        public static CompositeValidator CreateDefault(this ValidatorBuilder validatorBuilder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(DefaultValidationSettings.MinLettersCountInName, DefaultValidationSettings.MaxLettersCountInName)
                .ValidateLastName(DefaultValidationSettings.MinLettersCountInName, DefaultValidationSettings.MaxLettersCountInName)
                .ValidateDateOfBirth(DefaultValidationSettings.MinDateOfBirth)
                .ValidateSex()
                .ValidateSalary(DefaultValidationSettings.MinSalary, DefaultValidationSettings.MaxSalary)
                .ValidateYearsOfService(DefaultValidationSettings.MinYearsOfService, DefaultValidationSettings.MaxYearsOfService)
                .Create();
        }

        /// <summary>
        /// Creates composite validator with custom validation settings.
        /// </summary>
        /// <param name="validatorBuilder">Validator Builder.</param>
        /// <returns>Composite validator.</returns>
        public static CompositeValidator CreateCustom(this ValidatorBuilder validatorBuilder)
        {
            return new ValidatorBuilder()
                .ValidateFirstName(CustomValidationSettings.MinLettersCountInName, CustomValidationSettings.MaxLettersCountInName)
                .ValidateLastName(CustomValidationSettings.MinLettersCountInName, CustomValidationSettings.MaxLettersCountInName)
                .ValidateDateOfBirth(CustomValidationSettings.MinDateOfBirth)
                .ValidateSex()
                .ValidateSalary(CustomValidationSettings.MinSalary, CustomValidationSettings.MaxSalary)
                .ValidateYearsOfService(CustomValidationSettings.MinYearsOfService, CustomValidationSettings.MaxYearsOfService)
                .Create();
        }
    }
}
