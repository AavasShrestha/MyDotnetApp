using Microsoft.AspNetCore.Http;
using Sample.Data.DTO;
using Sample.Data.RoutingDB;
using Sample.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Service.Service.LogoService
{
    public class LogoService : ILogoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ValidationDTO DeleteLogo(int id)
        {
            var response = new ValidationDTO();
            try
            {
                if(id == 0)
                {
                    throw new ArgumentException("invalid logo id");
                }
                var entity = _unitOfWork.LogoRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    throw new ArgumentException("Logo not found");
                }

                _unitOfWork.LogoRepository.Delete(entity);
                _unitOfWork.Commit();

                //response.IsSuccess = true;
                //response.Message = "User deleted successfully";
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public IEnumerable<Logo> GetAllLogos()
        {
            try
            {
                return _unitOfWork.LogoRepository.GetQuerable().ToList();
            }
            catch (Exception)
            {
                return new List<Logo>();
            }
        }

        public Logo GetLogoById(int id)
        {
            try
            {
                if (id <= 0)
                    return null;

                return _unitOfWork.LogoRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ValidationDTO UpdateLogo(int id, IFormFile file)
        {
            var response = new ValidationDTO();
            try
            {
                if (id <= 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid logo id";
                    return response;
                }

                var entity = _unitOfWork.LogoRepository.GetQuerable(u => u.Id == id).FirstOrDefault();
                if (entity == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Logo not found";
                    return response;
                }

                // Generate a unique filename
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "logos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, newFileName);

                // Save file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Update entity
                entity.FileName = newFileName;
                entity.FilePath = $"/uploads/logos/{newFileName}";
                entity.CreatedDate = DateTime.Now;

                _unitOfWork.LogoRepository.Update(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "Logo updated successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public ValidationDTO UploadLogo(IFormFile file)
        {

            var response = new ValidationDTO();

            try
            {
                if (file == null || file.Length == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid file";
                    return response;
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "logos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }


                var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolder, newFileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Create new Logo entity
                var entity = new Logo
                {
                    FileName = newFileName,
                    FilePath = $"/uploads/logos/{newFileName}",
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1
                };

                _unitOfWork.LogoRepository.Add(entity);
                _unitOfWork.Commit();

                response.IsSuccess = true;
                response.Message = "Logo uploaded successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
