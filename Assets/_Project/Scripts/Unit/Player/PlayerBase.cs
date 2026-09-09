using Lean.Pool;
using TheWarOfAddicts.GameFlow;
using TheWarOfAddicts.GameFlow.InGame;
using TheWarOfAddicts.UI.InGame;
using TheWarOfAddicts.UI.InGame.Map;
using TheWarOfAddicts.Unit.Player.Weapon;
using TheWarOfAddicts.Unit.UnitManage;
using UnityEngine;

namespace TheWarOfAddicts.Unit.Player
{
    public abstract class PlayerBase: Unit
    {
        [SerializeField] private SpriteRenderer weaponRenderer;


        public bool HasUsingWeapon => usingWeapon;
        
        
        private WeaponData usingWeapon = null;
        
        
        void Update()
        {
            if (IsLocked) return;
            
            Move();
        }


        protected override void Init(UnitData data, OwnerType ownerType)
        {
            if (IsAlive) return;
            
            base.Init(data, ownerType);
            
            HpBar = HpBarManager.Instance?.GetHpBar(this);
            
            HpBar?.Refresh(CurrentHp, MaxHp);
            // TODO: onDamaged에 카메라 흔들림 등의 추가 모션 필요.
            //HpBar.FillColor = Color.blue;
                
            MapIcon = MapManager.Instance?.SetMapIcon(transform, Color.green, 2f);
        }

        protected override void InitStatus(UnitBaseData baseData, UnitBaseData upgradeData)
        {
            PlayerData pData = PlayerData.Instance;
            
            Level = pData.PlayerLevel;
            
            MaxHp = (int)CalcStatus(baseData.maxHp * pData.PlayerHpRate, upgradeData.maxHp, Level);
            Damage = (int)CalcStatus(baseData.damage, upgradeData.damage, Level);
            Armor = (int)CalcStatus(baseData.armor, upgradeData.armor, Level);
            AttackMinRange = CalcStatus(baseData.attackMinRange, upgradeData.attackMinRange, Level);
            AttackMaxRange = CalcStatus(baseData.attackMaxRange, upgradeData.attackMaxRange, Level);
            SkillRate = CalcStatus(baseData.skillRate, upgradeData.skillRate, Level);

            ImpaleCount = (int)CalcStatus(baseData.impaleCount, upgradeData.impaleCount, Level);
            ProjectileSpeed = CalcStatus(baseData.projectileSpeed, upgradeData.projectileSpeed, Level);
            
            MoveSpeed = CalcStatus(baseData.moveSpeed * pData.PlayerMoveSpeedRate, upgradeData.moveSpeed, Level);
            AttackSpeed = CalcStatus(baseData.attackSpeed, upgradeData.attackSpeed, Level);
            
            CurrentHp = MaxHp;
        }
        
        
        public void UseWeapon(WeaponData data)
        {
            usingWeapon = data;
            if (usingWeapon)
            {
                weaponRenderer.sprite = usingWeapon.handedSprite;
                Animator.SetInteger("State", (int)usingWeapon.animArmType);
            }
        }
        
        public override void Attack(Unit target)
        {
            
        }

        protected override void OnDie()
        {
            BattleResultManager.Instance.ProcessDefeat();
        }

        private void Move()
        {
            float x = Input.GetAxis("Horizontal");
            
            Animator.SetBool("IsMove", Mathf.Abs(x) > 0.05f);
            transform.Translate(x * MoveSpeed * Time.deltaTime, 0, 0);
            
            float posX = Mathf.Clamp(transform.position.x, -InGameManager.Instance.StageData.mapWidth * 0.5f, InGameManager.Instance.StageData.mapWidth * 0.5f);
            transform.position = new Vector3(posX, transform.position.y, transform.position.z); 
        }


        public void OnAnimWeapon()
        {
            Animator.SetInteger("State", 0);

            Vector2 pos = weaponRenderer.transform.position;
            var projectile = LeanPool.Spawn(usingWeapon.prefab, pos, Quaternion.identity);
            projectile.Init(pos + Vector2.right * 10 - Vector2.up, usingWeapon.Influence, usingWeapon.ImpaleCount, usingWeapon.ProjectileSpeed, Owner);
            
            usingWeapon = null;
        }
    }
}