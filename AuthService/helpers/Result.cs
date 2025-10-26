using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.helpers
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } //el mensaje de error o de éxito
        public T? Data { get; set; } //el dato que se retorna

        public static Result<T> Ok(T data) => new() { Success = true, Data = data }; //retorna un resultado exitoso con datos
        public static Result<T> Fail(string message) => new() { Success = false, Message = message }; //retorna un resultado fallido con mensaje de error

    }
}