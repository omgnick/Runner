using UnityEngine;
using System.Collections;

public class ButtonsScroller : BaseGuiElement {

	public UIButton scroll_left;
	public UIButton scroll_right;
	public UIScrollView scroll_view;
	public UIPanel panel;
	public UIGrid grid;

	public bool update_scroll_bars = false;

	//If smart_scroll enabled, scrolling is done by the value of one element or it's hidden part.
	//Also if scroll value for visible part of the element is greater then double_step_percentage, 
	//next element will be shown.
	public bool smart_scroll = true;
	public float scroll_time = 0.5f;
	public float distance_between_items = 40f;

	//If scroll value for visible part of the element is greater then double_step_percentage, 
	//next element will be shown. Work only with smart scroll enabled.
	public float double_step_percentage = 0.75f;

	private float visible_items_number;
	private float item_scroll_value;
	private float drag_from;
	private float drag_to;
	private float delta = 2f;
	private float delta_step;
	private float scroll_margin_left = 0f;



	public void Start () {
		SetupButton(scroll_left, "on_scroll_left");
		SetupButton(scroll_right, "on_scroll_right");

		if(panel == null)
			panel = GetComponent<UIPanel>();

		if(scroll_view == null)
			scroll_view = GetComponent<UIScrollView>();

		if(grid == null)
			grid = panel.GetComponentInChildren<UIGrid>();

		if(panel == null || scroll_view == null || grid == null){
			enabled = false;
			Debug.Log("ButtonsScroller was disabled because some of the components is null.");
			return;
		}
	}



	protected virtual void recalc_visible_items_number(){
		float item_width = grid.cellWidth;
		float total_width = panel.finalClipRegion.z;

		visible_items_number = total_width / item_width;
	}



	private void recalc_scroll_margin(){
		float visible_part = visible_items_number - Mathf.Floor(visible_items_number);
		scroll_margin_left = visible_part * item_scroll_value;
	}



	protected virtual void recalc_item_scroll_value(){
		if(items_count > visible_items_number){
			float item_width = grid.cellWidth;

			//The clipping doesn't show empty space of the last element, that is why distance_between_items is substracted.
			float part_to_scroll = (items_count - visible_items_number - distance_between_items / item_width);

			if(part_to_scroll < 1f)
				item_scroll_value = 1f;
			else
				item_scroll_value = 1f / part_to_scroll;

		} else
			item_scroll_value = 0f;
	}



	protected virtual void recalc_drag_from(){
		float item_width = grid.cellWidth;
		float clip_offset = panel.clipOffset.x;

		//The clipping doesn't show empty space of the last element, that is why distance_between_items is substracted.
		float hidden_items_width = (item_width * items_count - panel.finalClipRegion.z - distance_between_items);

		if(hidden_items_width <= 0)
			drag_from = drag_to;
		else
			drag_from = clip_offset / hidden_items_width;
	}



	public void Update () {
		if(is_scrolling)
			if(is_scroll_interrupted)
				stop_scrolling();
			else
				proceed_scrolling();
	}



	private void refresh_values(){
		recalc_visible_items_number();
		recalc_item_scroll_value();
		recalc_scroll_margin();
		recalc_drag_from();
		delta = 0f;
		delta_step = Time.smoothDeltaTime / scroll_time;
	}



	private void on_scroll_left(){
		if(scroll_view.shouldMoveHorizontally){

			refresh_values();

			/*In case if player draged the grid and some element is visible for less then double_step_percentage
			 * it will scroll until it is opened, otherwise will open the hole element and the next one.*/
			if(smart_scroll){
				drag_to = drag_from - (drag_from % item_scroll_value);

				if(drag_from - drag_to <= item_scroll_value * (1f - double_step_percentage))
					drag_to -= item_scroll_value;
			} else 
				drag_to = drag_from - item_scroll_value;
		}
	}



	private void on_scroll_right(){
		if(scroll_view.shouldMoveHorizontally){

			refresh_values();

			/*In case if player draged the grid and some element is visible for less then double_step_percentage 
			 * it will scroll until it is opened, otherwise will open the hole element and the next one.*/
			if(smart_scroll) {
				drag_to = (drag_from < item_scroll_value - scroll_margin_left ? item_scroll_value - scroll_margin_left : drag_from) 
					+ (item_scroll_value  - (Mathf.Abs(drag_from) % item_scroll_value));
				
				drag_from = Mathf.Max(0, drag_from);

				if(drag_to - drag_from <= item_scroll_value * (1f - double_step_percentage))
					drag_to += item_scroll_value;
			} else 
				drag_to = drag_from + item_scroll_value;
		}
	}



	private float items_count {
		get{
			return grid.transform.childCount;
		}
	}



	private bool is_scroll_interrupted {
		get{
			return scroll_view.isDragging;
		}
	}



	private bool is_scrolling {
		get{
			return delta <= 1f;
		}
	}



	private void stop_scrolling(){
		delta = 2f;
	}



	private void proceed_scrolling(){
		delta += delta_step;
		scroll_view.SetDragAmount(Mathf.Lerp(drag_from, drag_to, delta), 0, update_scroll_bars);
	}

	public void set_scrolling(float _x){
		delta = 1f;
		drag_to = _x;
	}

	public void refresh(){
		scroll_left.gameObject.SetActive(scroll_view.shouldMoveHorizontally);
		scroll_right.gameObject.SetActive(scroll_view.shouldMoveHorizontally);
	}
}
