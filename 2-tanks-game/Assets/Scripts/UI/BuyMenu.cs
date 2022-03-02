using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyMenu : MonoBehaviour
{
    // Container which holds contents of buy menu, to be turned on and off
    private Transform Container;

    // Template to add items to shop
    private Transform ShopItemTemplate;

    // The missile manager
    private MissileManager missileManager;

    // The tanks
    private Tank Tank1;

    private Tank Tank2;

    // The turn manager
    private TurnManager turnManager;

    private string currentMissileToBuy = "Basic Missile";

    public GameObject ToolTipPanel;

    public GameObject ToolTipText;

    private Dictionary<string, string> MissileTips = new Dictionary<string, string>()
    {
        {"Basic Missile","Average damage and average terrain destruction" },
        {"Miner Missile","Low damage but big terrain destruction" },
        {"Sniper Missile","High damage but you have to hit your shots" },
        {"Whale","When whale is in air, click to birth fishies" },
        {"Summon Death", "Summon the Bringer of Death" },
        {"Mountain Maker", "Spawns a mountain where it lands" },
        {"Blue Angry Bird", "Click to split into 3 birds- just like the mobile game" },
        {"Portal Gun", "Portals your tank to wherever this projectile lands" }
    };


    private void Awake()
    {
        Container = transform.Find("Container");
        ShopItemTemplate = Container.Find("ShopItemTemplate");
        ShopItemTemplate.gameObject.SetActive(false);
        missileManager = FindObjectOfType<MissileManager>();
        turnManager = FindObjectOfType<TurnManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Index for looping
        int index = 0;
        
        // Loop thru missiles and build buy menu
        foreach (MissileManager.missileInfo missileInfo in missileManager.missileArray)
        {
            CreateItemButton(
                missileInfo.missile.GetComponent<SpriteRenderer>().sprite,
                missileInfo.name,
                (int)missileInfo.cost,
                index);
            index++;
          
        }

        // Remove buy menu to start
        Button[] buttons = FindObjectsOfType<Button>();
        Container.gameObject.SetActive(false);
        ToolTipPanel.SetActive(false);
        foreach (Button b in buttons)
        {
             b.onClick.AddListener(ClickEvent);    
        }

    }

    // Update is called once per frame
    void Update()
    {
        Tank1 = GameObject.Find("Tank1").GetComponent<Tank>();
        Tank2 = GameObject.Find("Tank2").GetComponent<Tank>();

        // Close and bring up when B is pressed
        if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape))
            && Container.gameObject.activeSelf
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            Container.gameObject.SetActive(false);
            ToolTipPanel.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.B)
            && !Container.gameObject.activeSelf
            && GameObject.FindGameObjectWithTag("Projectile") == null)
        {
            Container.gameObject.SetActive(true);
        }

        // Grey out missiles that we cannot afford
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button b in buttons)
        {
            if(b.name != "BuyButton" && b.name != "Player1BuyButton" && b.name != "Player2BuyButton" && b.name != "BuyMenuExitButton")
            {
                if (turnManager.IsPlayerTurn(1))
                {
                    if (missileManager.missiles[missileManager.missileObjects[b.name]] > Tank1.turnPoints)
                    {
                        b.interactable = false;
                    }
                    else
                    {
                        b.interactable = true;
                    }
                }
                else
                {
                    if (missileManager.missiles[missileManager.missileObjects[b.name]] > Tank2.turnPoints)
                    {
                        b.interactable = false;
                    }
                    else
                    {
                        b.interactable = true;
                    }
                }
            }

        }

        // If the player deselected, reset buy
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (turnManager.IsPlayerTurn(1))
            {
                currentMissileToBuy = Tank1.currentMissileName;
            }
            else
            {
                currentMissileToBuy = Tank2.currentMissileName;
            }
        }
    }

    // Function to create items in the buy menu based on template
    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {
        // Create item with proper name
        Transform shopItemTransform = Instantiate(ShopItemTemplate, Container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();
        shopItemTransform.name = itemName;

        // Specify position for item
        float shopItemHeight = 10f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex + 35);

        // Set fields of item to display properly
        shopItemTransform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("PriceText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        shopItemTransform.Find("ItemImage").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.gameObject.SetActive(true);

        // Add on click functionality
        shopItemTransform.GetComponent<Button>().onClick.AddListener(ClickEvent);
        
    }
    

    // ClickEvent to be called when item in buy menu is clicked on
    private void ClickEvent()
    {
        Tank1 = GameObject.Find("Tank1").GetComponent<Tank>();
        Tank2 = GameObject.Find("Tank2").GetComponent<Tank>();
        // Get name of button clicked on, and attempt to purchase it for the correct tank
        string selectedButton = EventSystem.current.currentSelectedGameObject.name;
        if(selectedButton == "BuyButton")
        {
            if (turnManager.IsPlayerTurn(1) && GameObject.FindGameObjectWithTag("Projectile") == null)
            {
                Tank1.AttemptPurchaseMissile(currentMissileToBuy);
                Container.gameObject.SetActive(false);
            }
            else
            {
                Tank2.AttemptPurchaseMissile(currentMissileToBuy);
                Container.gameObject.SetActive(false);
            }
        }
        else
        {
            currentMissileToBuy = selectedButton;
        }
       
    }

    // Display tooltip method
    public void EnterToolTip(string selectedButton)
    {
        if (turnManager.IsPlayerTurn(2)) // Tooltip appears on players relative sides
        {
            ToolTipPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(650, -165);
        }
        else
        {
            ToolTipPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(-650, -165);
        }
        ToolTipPanel.SetActive(true);
        TextMeshProUGUI textMesh2 = ToolTipText.GetComponent<TextMeshProUGUI>();
        textMesh2.text = selectedButton.ToUpper() + ":\n" + MissileTips[selectedButton];
    }

    // Remove tooltip method
    public void ExitToolTip()
    {
        ToolTipPanel.SetActive(false);
    }
}
