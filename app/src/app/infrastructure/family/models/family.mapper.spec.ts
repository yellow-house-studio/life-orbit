import { 
  mapFamilyMemberResponseToModel, 
  mapAllergyResponseToModel,
  mapSafeFoodResponseToModel,
  mapFoodPreferenceResponseToModel
} from './family.mapper';
import { AllergenSeverity, FoodPreferenceStatus } from './family.model';
import { FamilyMemberResponse } from './family.dto';

describe('Family Mappers', () => {
  describe('mapAllergyResponseToModel', () => {
    it('should map allergy response to model', () => {
      const response = {
        allergen: 'peanuts',
        severity: 'NotAllowed' as const
      };

      const result = mapAllergyResponseToModel(response);

      expect(result).toEqual({
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      });
    });
  });

  describe('mapSafeFoodResponseToModel', () => {
    it('should map safe food response to model', () => {
      const response = {
        foodItem: 'pasta'
      };

      const result = mapSafeFoodResponseToModel(response);

      expect(result).toEqual({
        foodItem: 'pasta'
      });
    });
  });

  describe('mapFoodPreferenceResponseToModel', () => {
    it('should map food preference response to model', () => {
      const response = {
        foodItem: 'rice',
        status: 'Include' as const
      };

      const result = mapFoodPreferenceResponseToModel(response);

      expect(result).toEqual({
        foodItem: 'rice',
        status: FoodPreferenceStatus.Include
      });
    });
  });

  describe('mapFamilyMemberResponseToModel', () => {
    it('should map family member response to model', () => {
      const response: FamilyMemberResponse = {
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [
          { allergen: 'peanuts', severity: 'NotAllowed' }
        ],
        safeFoods: [
          { foodItem: 'pasta' }
        ],
        foodPreferences: [
          { foodItem: 'rice', status: 'Include' }
        ]
      };

      const result = mapFamilyMemberResponseToModel(response);

      expect(result).toEqual({
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [
          { allergen: 'peanuts', severity: AllergenSeverity.NotAllowed }
        ],
        safeFoods: [
          { foodItem: 'pasta' }
        ],
        foodPreferences: [
          { foodItem: 'rice', status: FoodPreferenceStatus.Include }
        ]
      });
    });

    it('should map empty arrays correctly', () => {
      const response: FamilyMemberResponse = {
        id: '123',
        name: 'John Doe',
        age: 30,
        allergies: [],
        safeFoods: [],
        foodPreferences: []
      };

      const result = mapFamilyMemberResponseToModel(response);

      expect(result.allergies).toEqual([]);
      expect(result.safeFoods).toEqual([]);
      expect(result.foodPreferences).toEqual([]);
    });
  });
}); 