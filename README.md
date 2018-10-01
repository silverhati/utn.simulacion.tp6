# utn.simulacion.tp6
***** Simulacion 2018 2C - TP 6: Software Factory *****

En una software factory, se brinda soporte y mantenimiento a distintos clientes.
En ella existen desarrolladores de distintas categorías: Junior (J), Semi Senior (SS), o Senior (S).

Una mejora sobre el sistema, o una falla a corregir, se trata en un ticket.
Los tickets están categorizados en relación a su prioridad: Baja, Normal o Alta.
Al momento de atender un nuevo ticket, los desarrolladores priorizarán los de prioridad Alta, luego los de Normal, y por último los de Baja.

Se conoce cada cuanto llega un requerimiento nuevo, es decir, el intervalo entre arribos de tickets.
Distribución: Baja 16,62%;  Normal 68,96%; Alta 14,42%.

También se conoce cuánto se tarda en resolver un ticket, una vez que es atendido por alguno de los desarrolladores
(3 TAs: uno correspondiente a S, otro a SS, y otro para J).

- Un ticket de prioridad ALTA va a ser tomado por un S, siempre que haya un S disponible. 
  En caso contrario, lo toma un SS. Y si tampoco hay SS disponible, lo toma un J.
- Un ticket de prioridad BAJA va a ser tomado por un J, siempre que haya un J disponible.
  En caso contrario, lo toma un SS. Y si tampoco hay SS disponible, lo toma un S.
- Un ticket de prioridad NORMAL va a ser tomado por un SS, siempre que haya un SS disponible.
  En caso contrario, lo toma un S. Y si tampoco hay S disponible, lo toma un J.
