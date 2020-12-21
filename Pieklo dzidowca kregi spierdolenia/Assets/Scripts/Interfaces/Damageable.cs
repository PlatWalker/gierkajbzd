///<summary>
///Created By Kumdzio
///</summary>


public interface Damageable{
    //każdy obiekt który będzie mógł być uszkodzony musi mieć metodę zwracającą
    //ile procentowo HP ma dany obiekt tak by obiekt wyświetlający pasek HP mógł pobrać HP
    float getHealthPercentage();
}
