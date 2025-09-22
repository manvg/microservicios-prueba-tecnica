using IntegracionAsistencia.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracionAsistencia.Domain.ValueObjects
{
    public class RutEmpleado
    {
        public string Valor { get; private set; }

        public RutEmpleado(string rut)
        {
            if (string.IsNullOrWhiteSpace(rut))
                throw new DomainException("El RUT no puede estar vacío");

            var rutLimpio = LimpiarRut(rut);

            if (!EsRutValido(rutLimpio))
                throw new DomainException($"El RUT {rut} no es válido");

            Valor = rutLimpio;
        }

        private static string LimpiarRut(string rut)
        {
            return rut.Replace(".", "").Replace("-", "").Replace(" ", "").ToUpper();
        }

        private static bool EsRutValido(string rut)
        {
            if (rut.Length < 2) return false;

            var cuerpo = rut[..^1];
            var digitoVerificador = rut[^1];

            if (!int.TryParse(cuerpo, out var numero)) return false;
            if (numero < 1000000) return false;

            return CalcularDigitoVerificador(numero) == digitoVerificador;
        }

        private static char CalcularDigitoVerificador(int rut)
        {
            var suma = 0;
            var multiplicador = 2;

            while (rut > 0)
            {
                suma += (rut % 10) * multiplicador;
                rut /= 10;
                multiplicador = multiplicador == 7 ? 2 : multiplicador + 1;
            }

            var resto = suma % 11;
            var digito = 11 - resto;

            return digito switch
            {
                11 => '0',
                10 => 'K',
                _ => char.Parse(digito.ToString())
            };
        }

        public string Formateado()
        {
            if (Valor.Length < 2) return Valor;

            var cuerpo = Valor[..^1];
            var dv = Valor[^1];

            var cuerpoFormateado = "";
            for (int i = cuerpo.Length - 1, contador = 0; i >= 0; i--, contador++)
            {
                if (contador > 0 && contador % 3 == 0)
                    cuerpoFormateado = "." + cuerpoFormateado;

                cuerpoFormateado = cuerpo[i] + cuerpoFormateado;
            }

            return $"{cuerpoFormateado}-{dv}";
        }

        public override string ToString() => Formateado();

        public override bool Equals(object obj)
        {
            if (obj is RutEmpleado otro)
                return Valor == otro.Valor;
            return false;
        }

        public override int GetHashCode()
        {
            return Valor?.GetHashCode() ?? 0;
        }
    }
}
