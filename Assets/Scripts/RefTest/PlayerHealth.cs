using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    //Player�̗̑͂ƃv���p�e�B
    private int playerHp = 100;
    private int maxHp = 1000;
    public int PlayerHp
    {
        get => playerHp;
        set => playerHp = Mathf.Min(playerHp += value, maxHp);
    }

    //��т̐��ƃv���p�e�B
    private int bandageCount;
    public int BandageCount { get => bandageCount; }

    //�򑐂̐��ƃv���p�e�B
    private int medicinalPlantscount;
    public int MedicinalPlantsCount { get => medicinalPlantscount; }

    //���ˊ�̐��ƃv���p�e�B
    private int syringeCount;
    public int SyringeCount { get => syringeCount; }

    // �񕜃A�C�e���̍ő吔(�S�A�C�e�����ʂ̏ꍇ)
    private const int maxCount = 100;

    // �Q�Ɩ߂�l�� switch ������ default ���莞�p�̕ϐ�
    private int x = 0;


    void Start() {
        Reset();

        // �f�o�b�O�p
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
    /// �񕜃A�C�e���̏��������X�V����
    /// ���̏���������΁A�����̉񕜃A�C�e���������Ă��A�A�C�e�����Ƃɓ����悤�ȃ��\�b�h��p�ӂ��Ȃ��Ă悢
    /// ����ăQ�[�����ɓo�ꂷ��A�C�e���̎�ނ��������Ă��A���������X�V���镔���̃v���O�����͈�ؕύX���s�v�ɂȂ�
    /// </summary>
    /// <param name="itemName">�A�C�e���̖��O</param>
    /// <param name="updateValue">�������̍X�V��</param>
    public void UpdateRecoveryItemCount(ItemName itemName, int updateValue) {
        // ItemName����X�V����񕜃A�C�e���̌��ƍő�l�� GetRecoveryItemCountRef ���\�b�h�ɎQ�Ɠn�������ď������A�߂�l���Q�Ɩ߂�l�ł�����Ď擾
        // �Q�Ƃ����l��߂�l�Ƃ��ĕϐ��ɑ�����Ă���̂ŁA����͐V�����l�ł͂Ȃ��A�����o�ϐ��ɗp�ӂ��Ă���ϐ��̏����Q�Ƃ����l���擾�ł���
        ref int recoveryItemCount = ref GetRecoveryItemCountRef(itemName);

        // TODO �񕜃A�C�e���̍ő及�������A�C�e�����ƂɈقȂ�ꍇ�ɂ́A�����ŗp�ӂ���

        // �񕜃A�C�e���̏��������A�ő�l�ƍŏ��l�Ɏ��܂�悤�ɐ������čX�V����
        recoveryItemCount = Mathf.Clamp(recoveryItemCount + updateValue, 0, maxCount);

        Debug.Log($"{ itemName } : { recoveryItemCount }");
    }

    /// <summary>
    /// �w�肵���񕜃A�C�e���̏��������Q�Ɩ߂�l�Ŏ擾����
    /// �A�C�e�����������茸�����肵���ꍇ�ɂ́A case �̕�����ǉ��E�폜����΁A��L�̏������̍X�V�̏������͉̂����ύX���Ȃ��Ă悢�B
    /// </summary>
    /// <param name="itemName">�񕜃A�C�e���̖��O</param>
    /// <returns>���̉񕜃A�C�e���̏�����</returns>
    private ref int GetRecoveryItemCountRef(ItemName itemName) {
        //�A�C�e���̖��O�ɉ����ď�����ύX
        switch (itemName) {
            case ItemName.Bandage: return ref bandageCount;
            case ItemName.MedicinalPlants: return ref medicinalPlantscount;
            case ItemName.Syringe: return ref syringeCount;
            default: return ref x;
        }
    }
}