using UnityEngine;
using System; // Event'ler için gerekli

// Bu script, oyunun genel durumunu, özellikle de oyuncunun coin miktarını yönetir.
// Singleton deseni kullanılarak, sahnenin herhangi bir yerinden kolayca erişilebilir olması sağlanır.
public class GameManagerVillage : MonoBehaviour
{
    // Singleton deseni için statik 'instance'
    private static GameManagerVillage _instance;
    public static GameManagerVillage Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager is NULL!");
            }
            return _instance;
        }
    }

    // Oyuncunun mevcut coin sayısı (private, dışarıdan değiştirilemez)
    private int _playerCoins = 0;
    public int PlayerCoins => _playerCoins; // Dışarıdan sadece okunabilir

    // Coin kazanıldığında tetiklenecek event. UI gibi diğer sistemler bunu dinleyecek.
    public static event Action<int> OnCoinUpdated;

    private void Awake()
    {
        // Sahnede başka bir GameManager varsa kendini yok et, yoksa bu objeyi 'instance' olarak ata.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            // DontDestroyOnLoad(this.gameObject); // Sahneler arası geçişte objenin korunması için. Şu anlık gerekmeyebilir.
        }
    }

    // Coin eklemek için kullanılacak public method.
    public void AddCoin(int amount)
    {
        _playerCoins += amount;
        Debug.Log($"Coin Kazanıldı! Toplam Coin: {_playerCoins}");

        // Event'i tetikle, dinleyen tüm methodları (örn: UI güncelleme) çağır.
        OnCoinUpdated?.Invoke(_playerCoins);
    }
}