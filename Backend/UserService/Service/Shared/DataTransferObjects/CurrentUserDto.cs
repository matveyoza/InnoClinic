namespace Service.Shared.DataTransferObjects
{
    namespace Service.Shared.DataTransferObjects
    {
        public record CurrentUserDto : UserDto
        {
            public string Role { get; set; } = string.Empty;
        }
    }
}
