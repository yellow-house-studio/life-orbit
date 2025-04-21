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
  FoodPreferenceResponse, 
  CreateFamilyMemberRequest,
  AllergyRequest,
  SafeFoodRequest,
  FoodPreferenceRequest,
  CreateCompleteFamilyMemberRequest
} from './family.dto';

export const mapAllergyResponseToModel = (response: AllergyResponse): Allergy => ({
  allergen: response.allergen,
  severity: response.severity === 'AvailableForOthers' ? AllergenSeverity.AvailableForOthers : AllergenSeverity.NotAllowed
});

export const mapSafeFoodResponseToModel = (response: SafeFoodResponse): SafeFood => ({
  foodItem: response.foodItem
});

export const mapFoodPreferenceResponseToModel = (response: FoodPreferenceResponse): FoodPreference => ({
  preference: response.foodItem,
  status: response.status === 'Include' ? FoodPreferenceStatus.Include :
         response.status === 'AvailableForOthers' ? FoodPreferenceStatus.AvailableForOthers :
         FoodPreferenceStatus.NotAllowed
});

export const mapFamilyMemberResponseToModel = (response: FamilyMemberResponse): FamilyMember => ({
  id: response.id,
  name: response.name,
  age: response.age,
  allergies: response.allergies.map(mapAllergyResponseToModel),
  safeFoods: response.safeFoods.map(mapSafeFoodResponseToModel),
  foodPreferences: response.foodPreferences.map(mapFoodPreferenceResponseToModel)
});

export const createFamilyMemeberRequest = (
  name: string,
  age: number,
  allergies: Allergy[],
  safeFoods: SafeFood[],
  foodPreferences: FoodPreference[]
): CreateCompleteFamilyMemberRequest => ({
  name,
  age,
  allergies: allergies.map(mapAllergyToAllergyRequest),
  safeFoods: safeFoods.map(mapSafeFoodToSafeFoodRequest),
  foodPreferences: foodPreferences.map(mapFoodPreferenceToFoodPreferenceRequest)
});

export const mapAllergyToAllergyRequest = (allergy: Allergy): AllergyRequest => ({
  allergen: allergy.allergen,
  severity: allergy.severity === AllergenSeverity.AvailableForOthers ? 'AvailableForOthers' : 'NotAllowed'
});

export const mapSafeFoodToSafeFoodRequest = (safeFood: SafeFood): SafeFoodRequest => ({
  foodItem: safeFood.foodItem
});

export const mapFoodPreferenceToFoodPreferenceRequest = (foodPreference: FoodPreference): FoodPreferenceRequest => ({
  foodItem: foodPreference.preference,
  status: foodPreference.status === FoodPreferenceStatus.Include ? 'Include' :
          foodPreference.status === FoodPreferenceStatus.AvailableForOthers ? 'AvailableForOthers' :
          'NotAllowed'
});



