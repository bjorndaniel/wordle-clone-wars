namespace WordleCloneWars.Models;

public class UpdateUserModel
{
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string RepeatPassword { get; set; } = string.Empty;

    public void ResetPasswordFields()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        RepeatPassword = string.Empty;
    }

    public bool HasUpdatedPassword() =>
        !(string.IsNullOrWhiteSpace(CurrentPassword) || string.IsNullOrWhiteSpace(NewPassword) ||
          string.IsNullOrWhiteSpace(RepeatPassword));

    public bool PasswordsEqual() =>
        NewPassword.Equals(RepeatPassword) && HasUpdatedPassword();

}