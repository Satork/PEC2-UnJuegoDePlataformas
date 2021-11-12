# PEC2-UnJuegoDePlataformas
Este proyecto es una recreación del nivel 1-1 del Super Mario Bros, juego original de Nintendo. 
Para ello se han usado Sprites muy similares a los originales que se encuentran aquí.

## **Contenido**

El juego consta de un jugador (Mario) que puede moverse de izquierda a derecha usando las teclas '**A**' (izquierda) y
'**S**' (derecha). Además, usando la tecla '**SPACE**' puede saltar. El nivel lo componen: bloques ladrillo, bloques interrogantes,
enemigos (Goombas) y el suelo base (incluyendo las tuberías).

### _**Bloques Ladrillo_**

Los bloques ladrillo son bloques que no proporcionan ninguna recompensa. Si Mario salta por debajo, éstos rebotan
y mandan a Mario con un rebote hacia abajo. Cuando Mario se convierte en Super, al colisionar por debajo con el bloque
ladrillo, lo rompe.

Los bloques ladrillo están posicionados en un **Tilemap** independiente. Al reconocer una colisión por debajo, si el objeto
es el jugador, cambia el **Tile** de la colisión por un GameObject con el mismo Sprite. Luego, efectúa una corutina que
mueve el bloque hasta una pequeña altura y luego lo vuelve a poner en su posición original. Cuando termina borra el
bloque ladrillo y vuelve a colocar el **Tile** en su posición original. Cuando el modo Super de Mario está activado,
el Tile se destruye (se cambia a _null_).

### **_Bloques Interrogantes_**

Los bloques interrogantes son bloques que al colisionar con ellos por debajo *dropean* una recompensa. Ésta
recompensa puede ser o bien un *Power-Up* o bien una moneda. El único *power-up* actualmente implementado es
la seta que convierte a Mario en Super, pero la estrella está lista para ser implementada, así como la flor (por
falta de tiempo no pudieron ser implementadas). En caso de *dropear* una seta, ésta se mueve por el escenario,
interactuando sólo con la plataforma base para cambiar de dirección. En caso de caer al vacío, se pierde la seta.

Los bloques interrogantes, al igual que los *power-ups* vienen en sus respectivos **Tilemaps** independientes. En este
caso, se usa la información del **Tilemap** de los *power-ups* para saber qué recompensa se debe dar en cada bloque
interrogante. En caso de que el jugador colisione por debajo con un bloque interrogación, el bloque guarda su
posición en el **Grid** y busca esa posición en el *Power-Up* Tilemap. Para *spawnear* la recompensa, se usan **Scriptable
Objects** (guardados bajo el nombre de **TileData**), que almacenan el **Tile** con el que comparar, el **GameObject** a *spawnear*, la velocidad de aparición, y (en caso
de que sea mueva) una velocidad de movimiento. Una vez se ha comparado el **Tile** con la posición de colisión, se
crea una instancia del **GameObject** almacenado el **TileData**. A ese **GameObject**, se le pasa la información del **TileData**.

En el caso de las monedas, cuando aparecen, se mueven desde la posición del **Tile** hasta una posición por encima en el
Grid. Una vez alcanzada la posicón, se destruyen. En el caso de la seta, sucede lo mismo, excepto que en vez de
destruirse el objecto, se le agrega un Rigidbody2D y se le asigna un movimiento. Si colisiona lateralmente con alguna
plataforma, cambia de dirección. Si el jugador entra en contacto con la seta, ésta le proporciona el estado de Super,
incrementando su tamaño y proporcionando protección extra en caso de colisión con un enemigo.

En este caso, cómo también existían recompensas en otros bloques que no fueran "bloques interrogantes", añadí que los
bloques interrogantes también podían ser determinados bloques ladrillos siempre que éstos se encontraran dentro del
mismo **Tilemap**. Al colisionar por debajo con los ?bloques, éstos se vuelven de otro color y no son otra vez 
interactuables.

### **_Enemigos - Goombas_**

Los enemigos actuales sólo son los Goombas. Éstos se encuentran en las posiciones originales del juego.
Interactúan con las plataformas como los _power-up_ y cambian de dirección cuando colisionan lateralmente con alguna
plataforma. En caso de que el jugador colisione con ellos, mata al jugador o, en caso de que éste esté en estado 
Super, reinvierte el estado a Normal. Si el jugador salta encima de los Goombas, los mata y el jugador da un pequeño
rebote hacia arriba. Los Goombas pueden ser eliminados por el jugador o caer al vacío. En ambos casos, son destruidos.

### **_Plataforma Base_**

La plataforma base es igual a la original del juego. Sin embargo, por falta de tiempo, no se ha podido implementar
el sitio secreto por el que se puede acceder en la 4ª tubería y salir por la penúltima tubería. Cuenta con espacios
que dan al vacío y que suponen la muerte del jugador. También cuenta con una línea de meta representada con una
mástil sujetando una bandera. Al llegar a la meta, la bandera baja y aparece la pantalla de victoria. Si el jugador pulsa
cualquier tecla, se volverá a empezar el juego. Cuando el jugador muere, aparece una pantalla de **Game Over**. Si el
jugador pulsa '**R**' vuelve a empezar el juego.

## **Referencias**

Los sprites usados fueron sacadas de [Mario Universe](http://www.mariouniverse.com/sprites-nes-smb/)
- Los sprites de Mario que se usaron fueron [éstos](http://www.mariouniverse.com/wp-content/img/sprites/nes/smb/mario.png), pero no hay ninguna referencia al autor.
- El resto de los sprites usados fueron gracias a JoevTheRaviol por [éstos](http://www.mariouniverse.com/wp-content/img/sprites/nes/smb/misc-custom.png) resprites.