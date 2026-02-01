namespace Domain.Enums;

public enum UserRole
{
    Admin = 1,
    User = 2
}

public enum RentalStatus
{
    Active = 1,
    Completed = 2,
    Canceled = 3,
}

public enum EmailStatus
{
    Available = 1,      // Свободен email
    AlreadyExists = 2,  // Уже есть такой email
    NotAllowed = 3      // Этот email не подойдёт
}
