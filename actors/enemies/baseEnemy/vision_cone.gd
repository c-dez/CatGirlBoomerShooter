extends Area3D
class_name VisionCone

@onready var ray :RayCast3D = get_node("RayCast3D")





func _physics_process(delta: float) -> void:
    
    pass

func _on_body_entered(body:Node3D) -> void:
    if body.is_in_group("Player"):

        print("player detected")
    pass