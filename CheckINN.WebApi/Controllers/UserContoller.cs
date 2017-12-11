using System;
using System.Net;
using System.Web.Http;
using CheckINN.Repository.Repositories;
using CheckINN.WebApi.Entities;
using log4net;

namespace CheckINN.WebApi.Controllers
{
    public class UserContoller : ApiController
    {
        private readonly UserRepository _userRepository;
        private readonly ILog _log;

        public UserContoller(UserRepository userRepository, ILog log)
        {
            _userRepository = userRepository;
            _log = log;
        }

        [HttpPost] public Status CreateUser([FromBody] User user)
        {
            try
            {
                _userRepository.NewUser(user.Username, user.Password);
            }
            catch (Exception e)
            {
                _log.Error(e);
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return new Status(true, "User created");
        }
    }
}
