# Fíbula — Character Combat Prototype 07

Prototype 07 é o Prototype 06 com uma única mudança de comportamento:
a duração do voo da arma agora é proporcional à distância inicial até o alvo.

## Duração proporcional à distância

A duração é calculada uma única vez no começo de cada ataque.

Fórmula conceitual:

`duração = distância inicial / velocidade`

Depois ela é limitada pelos valores mínimo e máximo configurados no Inspector.

### Parâmetros

- `Attack Speed`: velocidade-base da arma em unidades por segundo.
- `Min Attack Duration`: duração mínima do voo.
- `Max Attack Duration`: duração máxima do voo.

Valores iniciais do Prototype 07:

- Attack Speed: `12`
- Min Attack Duration: `0.20`
- Max Attack Duration: `0.60`

A duração não é recalculada durante o voo.

Isso é importante porque o Prototype 06 possui perseguição dinâmica: o alvo pode se mover enquanto a espada está voando. Recalcular a duração por frame poderia alterar o `t` durante o ataque e produzir movimento inconsistente.

## Perseguição continua funcionando

A posição do alvo continua sendo consultada a cada frame.

Portanto:

- a distância inicial define a duração;
- a posição atual do alvo define para onde a espada está indo.

Essas duas responsabilidades ficam separadas.

## O que permanece igual

- perseguição dinâmica do Prototype 06;
- dano garantido ao final do ataque;
- retorno dinâmico;
- orientação da arma;
- callback normal;
- callback de cancelamento;
- `OnDisable()`;
- `PerformAttack()` retornando `bool`;
- proteção contra arma inativa;
- gesto da mão;
- uma arma;
- ataque por SPACE;
- arma independente na hierarquia;
- sem hit detection física.

## Teste recomendado

### Teste 1 — alvo perto

1. Coloque o alvo próximo.
2. Pressione SPACE.
3. Observe que o ataque é mais rápido.

### Teste 2 — alvo longe

1. Afaste o alvo.
2. Pressione SPACE.
3. Observe que o ataque demora mais.

### Teste 3 — limite mínimo

Coloque o alvo muito perto e confirme que o ataque não fica instantâneo.

### Teste 4 — limite máximo

Coloque o alvo muito longe e confirme que a duração não ultrapassa `Max Attack Duration`.

### Teste 5 — alvo em movimento

Mova o alvo durante o ataque.

A duração daquele ataque deve continuar a mesma, enquanto a trajetória continua acompanhando o alvo.

### Teste 6 — regressão do 05/06

Repita:

- ataque normal;
- cancelar/desativar espada durante ataque;
- reativar espada;
- atacar novamente;
- pressionar SPACE com espada desativada.

O controller não deve ficar preso.

## Próxima etapa

Hit detection física e sincronização do gesto da mão continuam adiadas.