import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of, throwError } from 'rxjs';
import { FamilyApiService } from '../services/family-api.service';
import { FamilyMembersStore } from './family-members.store';
import { CreateFamilyMemberRequest } from '../models/family.dto';
import { FamilyMember, AllergenSeverity, FoodPreferenceStatus } from '../models/family.model';
import { mockProvider } from '@ngneat/spectator/jest';
import type { SpyObject } from '@ngneat/spectator/jest';
import { provideTestLogger } from '../../common/logging/test-logger';

describe('FamilyMembersStore', () => {
  let spectator: SpectatorService<FamilyMembersStore>;
  let store: FamilyMembersStore;
  let apiService: SpyObject<FamilyApiService>;

  const mockFamilyMember: FamilyMember = {
    id: '123',
    name: 'John Doe',
    age: 30,
    allergies: [{ allergen: 'peanuts', severity: AllergenSeverity.NotAllowed }],
    safeFoods: [{ foodItem: 'pasta' }],
    foodPreferences: [{ foodItem: 'rice', status: FoodPreferenceStatus.Include }]
  };

  const createService = createServiceFactory({
    service: FamilyMembersStore,
    providers: [
      mockProvider(FamilyApiService, {
        getFamilyMembers: jest.fn().mockReturnValue(of([])),
        createFamilyMember: jest.fn().mockReturnValue(of('')),
        addAllergy: jest.fn().mockReturnValue(of(void 0)),
        removeAllergy: jest.fn().mockReturnValue(of(void 0)),
        addSafeFood: jest.fn().mockReturnValue(of(void 0)),
        removeSafeFood: jest.fn().mockReturnValue(of(void 0)),
        addFoodPreference: jest.fn().mockReturnValue(of(void 0)),
        removeFoodPreference: jest.fn().mockReturnValue(of(void 0))
      }),
      provideTestLogger(true),
    ]
  });

  beforeEach(() => {
    spectator = createService();
    store = spectator.service;
    apiService = spectator.inject(FamilyApiService);

    jest.clearAllMocks();
  });

  it('should be created', () => {
    expect(store).toBeTruthy();
  });

  describe('loadFamilyMembers', () => {
    it('loads family members successfully', () => {
      apiService.getFamilyMembers.mockReturnValueOnce(of([mockFamilyMember]));

      store.loadFamilyMembers();

      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
      expect(store.loading()).toBe(false);
      expect(store.error()).toBeNull();
    });

    it('handles error when loading family members fails', () => {
      const error = new Error('Failed to load');
      apiService.getFamilyMembers.mockReturnValue(throwError(() => error));

      store.loadFamilyMembers();

      expect(store.familyMembers()).toEqual([]);
      expect(store.loading()).toBe(false);
      expect(store.error()).toBe(error.message);
    });
  });

  describe('createFamilyMember', () => {
    it('creates family member and reloads list', () => {
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };

      apiService.createFamilyMember.mockReturnValue(of('123'));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.createFamilyMember(request);

      expect(apiService.createFamilyMember).toHaveBeenCalledWith(request);
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.loading()).toBe(false);
      expect(store.error()).toBeNull();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });

    it('handles error when creating family member fails', () => {
      const error = new Error('Failed to create');
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };

      apiService.createFamilyMember.mockReturnValue(throwError(() => error));

      store.createFamilyMember(request);

      expect(store.loading()).toBe(false);
      expect(store.error()).toBe(error.message);
    });
  });

  describe('selectFamilyMember', () => {
    it('selects family member by id', () => {
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));
      
      store.loadFamilyMembers();
      store.selectFamilyMember(mockFamilyMember.id);
      
      expect(store.selectedFamilyMember()).toEqual(mockFamilyMember);
    });
  });

  describe('addAllergy', () => {
    it('adds allergy and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        allergen: 'peanuts',
        severity: AllergenSeverity.NotAllowed
      };

      apiService.addAllergy.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addAllergy(params);

      expect(apiService.addAllergy).toHaveBeenCalledWith(
        params.familyMemberId,
        params.allergen,
        params.severity
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });
  });

  describe('addSafeFood', () => {
    it('adds safe food and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        foodItem: 'pasta'
      };

      apiService.addSafeFood.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addSafeFood(params);

      expect(apiService.addSafeFood).toHaveBeenCalledWith(
        params.familyMemberId,
        params.foodItem
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });
  });

  describe('addFoodPreference', () => {
    it('adds food preference and reloads family members', () => {
      const params = {
        familyMemberId: '123',
        foodItem: 'rice',
        status: FoodPreferenceStatus.Include
      };

      apiService.addFoodPreference.mockReturnValue(of(void 0));
      apiService.getFamilyMembers.mockReturnValue(of([mockFamilyMember]));

      store.addFoodPreference(params);

      expect(apiService.addFoodPreference).toHaveBeenCalledWith(
        params.familyMemberId,
        params.foodItem,
        params.status
      );
      expect(apiService.getFamilyMembers).toHaveBeenCalled();
      expect(store.familyMembers()).toEqual([mockFamilyMember]);
    });
  });
}); 