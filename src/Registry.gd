# This is a workaround a weird bug that doesn't accept cross-language scripting for autoloaded scripts
extends Node

# Read


# Write
func create_agents(agentname, _type): $Khana.AddAgent(agentname)
func remove_agents(ID): $Khana.RemoveAgent(ID)
func conduct_transaction(ID, cash): $Khana.ConductTransaction(ID, cash)
func get_cash(ID): $Khana.GetAgentCash(ID)
func add_property(ID, prop): $Khana.AddProperty(ID, prop)
func remove_property(ID, prop): $Khana.RemoveProperty(ID, prop)
func change_mortgage(ID, prop, status): $Khana.ToggleMortgageStatus(ID, prop, status)
func modify_building(ID, prop, lvl): $Khana.ModifyUpgradeLvl(ID, prop, lvl)
