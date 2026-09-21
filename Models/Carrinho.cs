namespace cinemabackend.Models;

public class Carrinho
{
  public int Id_carrinho {get; private set;}
  public string Forma_pagamento {get; private set;}
  public decimal Desconto {get; private set;}
  public string Status {get; private set;}
  public TimeOnly Hora_pagamento {get; private set;}


  public Carrinho (int id_carrinho, string forma_pagamento, decimal desconto, string status, TymeOnly hora_pagamento )
  {
    Id_carrinho = id_carrinho;
    Forma_pagamento = forma_pagamento;
    Desconto = desconto;
    Status = status;
    Hora_pagamento = hora_pagamento;
    
  }
  
}
