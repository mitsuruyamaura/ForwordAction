using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    //Playerの体力とプロパティ
    private int playerHp = 100;
    private int maxHp = 1000;
    public int PlayerHp
    {
        get => playerHp;
        set => playerHp = Mathf.Min(playerHp += value, maxHp);
    }

    //包帯の数とプロパティ
    private int bandageCount;
    public int BandageCount { get => bandageCount; }

    //薬草の数とプロパティ
    private int medicinalPlantscount;
    public int MedicinalPlantsCount { get => medicinalPlantscount; }

    //注射器の数とプロパティ
    private int syringeCount;
    public int SyringeCount { get => syringeCount; }

    // 回復アイテムの最大数(全アイテム共通の場合)
    private const int maxCount = 100;

    // 参照戻り値の switch 文内の default 判定時用の変数
    private int x = 0;


    void Start() {
        Reset();

        // デバッグ用
        UpdateRecoveryItemCount(ItemName.Bandage, -1);
        UpdateRecoveryItemCount(ItemName.Bandage, -1);

        UpdateRecoveryItemCount(ItemName.MedicinalPlants, -2);
    }

    void Reset() {
        bandageCount = 10;
        medicinalPlantscount = 5;
        syringeCount = 3;
    } 

    /// <summary>
    /// 回復アイテムの所持数を更新する
    /// この処理があれば、複数の回復アイテムがあっても、アイテムごとに同じようなメソッドを用意しなくてよい
    /// よってゲーム内に登場するアイテムの種類が増減しても、所持数を更新する部分のプログラムは一切変更が不要になる
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    /// <param name="updateValue">所持数の更新量</param>
    public void UpdateRecoveryItemCount(ItemName itemName, int updateValue) {
        // ItemNameから更新する回復アイテムの個数と最大値を GetRecoveryItemCountRef メソッドに参照渡しをして処理し、戻り値を参照戻り値でもらって取得
        // 参照した値を戻り値として変数に代入しているので、それは新しい値ではなく、メンバ変数に用意している変数の情報を参照した値が取得できる
        ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);

        // TODO 回復アイテムの最大所持数がアイテムごとに異なる場合には、ここで用意する

        // 回復アイテムの所持数を、最大値と最小値に収まるように制限して更新する
        recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);

        Debug.Log($"{ itemName } : { recoveryItemCount }");
    }

    /// <summary>
    /// 指定した回復アイテムの所持数を参照戻り値で取得する
    /// アイテムが増えたり減ったりした場合には、 case の部分を追加・削除すれば、上記の所持数の更新の処理自体は何も変更しなくてよい。
    /// </summary>
    /// <param name="itemName">回復アイテムの名前</param>
    /// <returns>その回復アイテムの所持数</returns>
    private ref int GetRecoveryItemCountRef(ItemName itemName) {
        //アイテムの名前に応じて処理を変更
        switch (itemName) {
            case ItemName.Bandage: return ref bandageCount;
            case ItemName.MedicinalPlants: return ref medicinalPlantscount;
            case ItemName.Syringe: return ref syringeCount;
            default: return ref x;
        }
    }
}