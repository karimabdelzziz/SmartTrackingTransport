using Infrastucture.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services.TokenService
{
	public interface ITokenService
	{
		public string CreateToken(AppUser appUser);
	}
}
