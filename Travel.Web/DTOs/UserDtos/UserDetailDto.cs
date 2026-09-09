namespace Travel.Web.DTOs.UserDtos
{
    public class UserDetailDto
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TermsAccepted { get; set; }

        public DateTime? TermsAcceptedAt { get; set; }

        public string FullName =>
            $"{FirstName} {LastName}".Trim();
    }
}
