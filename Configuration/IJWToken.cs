using API.Identity.Entities;

namespace API.Configuration {
    public interface IJWToken {
        public string CreateToken(AppUser user);
    }
}