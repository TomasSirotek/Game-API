using API.Identity.Entities;

namespace API.Configuration {
    public interface IJWToken {
        public string CreateToken(List<string> roles, string userId, double duration);
    }
}