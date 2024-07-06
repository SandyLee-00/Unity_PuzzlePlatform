# ❄ Frozen Escape ❄
  
## 📋 목차
- 🖥 프로젝트 소개
- ⏲ 개발기간
- ⚙ 개발 환경
- 👪 팀원 및 역할
- 🖼 와이어 프레임
- 📹 시연 영상
- 🎮 조작 방법
- 📖 주요 기능
- 📌 에셋 출처

## 🖥 프로젝트 소개
- 미끄러운 지형과 장애물들을 피하여 목표지점까지 이동하는 3D 플래포머 게임 

## ⏲ 개발기간
- 2024.06.03(월) ~ 2024.06.10(월)

## ⚙ 개발 환경
- **Game Engine**: Unity (2022.3.17f1)
- **IDE**: Visual Studio Community 2022
- **Language**: C#
- **VCS**: GitHub

## 👪 팀원 및 역할
### 팀장 : 진강산
- Item, SOData, Interaction, Inventory
  
### 팀원 : 이서영
- Player, Animation, Sound
  
### 팀원 : 탁혁재
- Platform, Data

### 팀원 : 최유정
- UI, GameManager, Inventory

## 🖼 와이어 프레임
![와이어프레임](https://github.com/SandyLee-00/Unity_PuzzlePlatformer/assets/159543415/f88ed30f-19e0-4338-9b99-f24041b3b53a)

## 📹 시연 영상
[(링크)](https://www.youtube.com/watch?v=Sffyg8J8WI4)

## 🎮 조작 방법
- **이동**: W, A, S, D

- **점프**: Space Bar

- **달리기**: Shift

- **시점 이동**: Mouse

- **상호작용**: E

- **인벤토리**: Tab

- **커서 락**: ESC

## 📖 주요 기능
### INTRO SCENE
- Play 버튼을 누르면 게임이 시작됩니다.
- Load Data 버튼을 누르면 저장된 데이터를 불러올 수 있습니다.
- Setting 버튼을 누르면 BGM과 SFX를 개별적으로 조절할 수 있습니다.
- Credit 버튼을 누르면 게임 제작자들을 볼 수 있습니다.
- Exit 버튼을 누르면 게임이 종료됩니다.

### PLAY SCENE
- 게임의 목표는 플레이어가 캐릭터를 조작하여 시작 지점에서 목표 지점까지 안전하게 이동하는 것입니다.
- 플레이어는 키보드와 마우스로 캐릭터를 조작할 수 있습니다.
- Shift를 눌러 달릴 때 캐릭터는 스태미나를 소모하며 스태미나를 모두 소비하면 더 이상 달릴 수 없습니다.
- 맵의 중간중간에는 아이템과 버프들이 존재하고, 습득하거나 장착하면 캐릭터의 능력치를 상승시켜 줍니다.
- 맵에는 상하 또는 좌우로 움직이는 플랫폼과 캐릭터에게 피해를 입히는 장애물들이 존재합니다.
- 캐릭터가 장애물과 충돌하거나 바닥으로 떨어지는 경우 체력이 감소하고 모든 체력이 감소되면 게임오버 됩니다.
- 게임오버가 되면 게임을 재시작하거나 INTRO SCENE으로 나갈 수 있습니다.
- 아이템, 버프 및 장애물에 크로스 헤어를 가져다 대면 해당 오브젝트의 이름과 설명을 UI로 보여줍니다.
- 인벤토리에서는 습득한 장작 아이템들을 볼 수 있고, 아이템을 클릭하면 해당 아이템의 이름, 설명, 증가 능력치를 볼 수 있고, 아이템을 탈착하거나 버릴 수 있습니다.
- 플레이어가 캐릭터를 안전하게 목표 지점까지 이동시키면 게임이 클리어 되고, 클리어 타임을 확인할 수 있고 게임을 재시작하거나 INTRO SCENE으로 나갈 수 있습니다.
- 플레이어는 게임 중간에 Pause 버튼을 눌러 게임을 일시 정지시킬 수 있습니다.
- Pause 패널에서는 게임을 계속 진행하거나, 현재 진행 상황을 저장하거나, INTRO SCENE으로 나갈 수 있습니다.

## 📌 에셋 출처
### Platform
- https://kenney.nl/assets/platformer-kit
- https://kenney.nl/assets/marble-kit

### Character
- https://kenney.nl/assets/animated-characters-2

### UI
- https://kenney.nl/assets/ui-pack-rpg-expansion

### Sound
- https://kenney.nl/assets/rpg-audio
- https://kenney.nl/assets/impact-sounds
- https://kenney.nl/assets/digital-audio
- https://kenney.nl/assets/ui-audio
- https://assetstore.unity.com/packages/audio/music/adventure-music-and-sfx-221545
