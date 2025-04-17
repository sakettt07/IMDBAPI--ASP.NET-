using System;

namespace IMDBAPI.Services.CustomException
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }
    }
}
