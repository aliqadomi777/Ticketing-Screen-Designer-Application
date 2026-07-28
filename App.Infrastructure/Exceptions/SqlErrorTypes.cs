namespace App.Infrastructure.Exceptions
{
    public enum SqlErrorTypes
    {
        UniqueConstraintViolation = 2627,
        UniqueIndexViolation = 2601,
        ForeignKeyViolation = 547
    }
}
