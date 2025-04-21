import { 
  FamilyMember, 
  Allergy, 
  SafeFood, 
  FoodPreference, 
  AllergenSeverity,
  FoodPreferenceStatus 
} from './family.model';
import { 
  FamilyMemberResponse, 
  AllergyResponse, 
  SafeFoodResponse, 
  FoodPreferenceResponse 
} from './family.dto';

export const mapAllergyResponseToModel = (response: AllergyResponse): Allergy => ({
  allergen: response.allergen,
  severity: response.severity as AllergenSeverity
});

export const mapSafeFoodResponseToModel = (response: SafeFoodResponse): SafeFood => ({
  foodItem: response.foodItem
});

export const mapFoodPreferenceResponseToModel = (response: FoodPreferenceResponse): FoodPreference => ({
  preference: response.foodItem,
  status: response.status as FoodPreferenceStatus
});

export const mapFamilyMemberResponseToModel = (response: FamilyMemberResponse): FamilyMember => ({
  id: response.id,
  name: response.name,
  age: response.age,
  allergies: response.allergies.map(mapAllergyResponseToModel),
  safeFoods: response.safeFoods.map(mapSafeFoodResponseToModel),
  foodPreferences: response.foodPreferences.map(mapFoodPreferenceResponseToModel)
}); 