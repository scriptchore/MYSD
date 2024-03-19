using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MYSD_REG.Core.Models;
using MYSD_REG.Core.Models.DTOs;
using MYSD_REG.Core.Models.DTOs.Response;
using MYSD_REG.Domain.Interfaces;
using MYSD_REG.Domain.Utilities;
using System.Net;

namespace MYSD_REG.Api.Controllers
{

    [Route("api/v1/[controller]")]
    [EnableCors("MYSD.API")]
    [ApiController]
    public class UserController : ControllerBase
    {

        readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(APIResponseDTO<ApplicationUserDTO>), 201)]
        [ProducesResponseType(typeof(APIResponseDTO<string>), 400)]
        [ProducesResponseType(typeof(APIResponseDTO<string>), 404)]
        [ProducesResponseType(typeof(APIResponseDTO<string>), 500)]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterApplicant([FromRoute] Title title, [FromRoute]  Gender gender, [FromRoute] MaritalStatus maritalStatus, [FromRoute] EmploymentStatus employmentStatus, [FromRoute] EmploymentSector sector, [FromRoute] BenefitProgramme benefitProgramme, [FromRoute] YesNo disability, [FromRoute] YesNo benefit, CreateApplicationDTO applicationUserVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, new APIResponseDTO<string>()
                    {
                        RespCode = "100",
                        RespMessage = ResponseCode.GetDescription("100")
                    });
                }

                ApplicationUserDTO newApplication = new ApplicationUserDTO();

                newApplication.Title = title.ToString();
                newApplication.Gender = gender.ToString();
                newApplication.MaritalStatus = maritalStatus.ToString();
                newApplication.EmploymentStatus = employmentStatus.ToString();
                newApplication.EmploymentSector = sector.ToString();
                newApplication.BenefitedFrom = benefitProgramme.ToString();
                newApplication.Disability = disability.ToString();


                newApplication.Firstname = applicationUserVM.Firstname;
                newApplication.Surname = applicationUserVM.Surname;
                newApplication.Othername = applicationUserVM.Othername;
                newApplication.DOB = applicationUserVM.DOB;
                newApplication.Address = applicationUserVM.Address;
                newApplication.LGofResidence = applicationUserVM.LGofResidence;
                newApplication.HighestEduQualification = applicationUserVM.HighestEduQualification;
                newApplication.EmployerName = applicationUserVM.EmployerName;
                newApplication.Email = applicationUserVM.Email;
                newApplication.Phone = applicationUserVM.PhoneNumber;
                newApplication.LASRAID = applicationUserVM.LASRAID; 
             



                var resp = await _service.CreateApplicationUserAsync(newApplication);

                if (!resp.RespCode.Equals("00"))
                {
                    return StatusCode((int)HttpStatusCode.NotFound, resp);
                }

                return StatusCode((int)HttpStatusCode.Created, resp);
            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, new APIResponseDTO<string>()
                {
                    RespCode = "xxx",
                    RespMessage = ResponseCode.GetDescription("xxx")
                });
            }
          
        }
    }
}
