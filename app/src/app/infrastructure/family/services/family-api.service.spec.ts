import { createHttpFactory, HttpMethod, SpectatorHttp } from '@ngneat/spectator/jest';
import { FamilyApiService } from './family-api.service';
import { CreateFamilyMemberRequest } from '../models/family.dto';

describe('FamilyApiService', () => {
  let spectator: SpectatorHttp<FamilyApiService>;
  const createHttp = createHttpFactory(FamilyApiService);

  beforeEach(() => spectator = createHttp());

  it('creates service instance', () => {
    expect(spectator.service).toBeTruthy();
  });

  describe('getFamilyMembers', () => {
    it('makes GET request to fetch all family members', () => {
      spectator.service.getFamilyMembers().subscribe();
      
      const req = spectator.expectOne('/settings/family', HttpMethod.GET);
      expect(req.request.params.keys()).toHaveLength(0);
    });
  });

  describe('getFamilyMember', () => {
    it('makes GET request to fetch specific family member', () => {
      const id = '123';
      
      spectator.service.getFamilyMember(id).subscribe();
      
      spectator.expectOne(`/settings/family/${id}`, HttpMethod.GET);
    });
  });

  describe('createFamilyMember', () => {
    it('makes POST request to create family member', () => {
      const request: CreateFamilyMemberRequest = {
        name: 'John Doe',
        age: 30
      };
      
      spectator.service.createFamilyMember(request).subscribe();
      
      const req = spectator.expectOne('/settings/family', HttpMethod.POST);
      expect(req.request.body).toEqual(request);
    });
  });

  describe('addAllergy', () => {
    it('makes POST request to add allergy', () => {
      const familyMemberId = '123';
      const allergen = 'peanuts';
      const severity = 'NotAllowed' as const;
      
      spectator.service.addAllergy(familyMemberId, allergen, severity).subscribe();
      
      const req = spectator.expectOne(`/settings/family/${familyMemberId}/allergies`, HttpMethod.POST);
      expect(req.request.body).toEqual({ allergen, severity });
    });
  });

  describe('removeAllergy', () => {
    it('makes DELETE request to remove allergy', () => {
      const familyMemberId = '123';
      const allergen = 'peanuts';
      
      spectator.service.removeAllergy(familyMemberId, allergen).subscribe();
      
      spectator.expectOne(`/settings/family/${familyMemberId}/allergies/${allergen}`, HttpMethod.DELETE);
    });
  });

  describe('addSafeFood', () => {
    it('makes POST request to add safe food', () => {
      const familyMemberId = '123';
      const foodItem = 'pasta';
      
      spectator.service.addSafeFood(familyMemberId, foodItem).subscribe();
      
      const req = spectator.expectOne(`/settings/family/${familyMemberId}/safefoods`, HttpMethod.POST);
      expect(req.request.body).toEqual({ foodItem });
    });
  });

  describe('removeSafeFood', () => {
    it('makes DELETE request to remove safe food', () => {
      const familyMemberId = '123';
      const foodItem = 'pasta';
      
      spectator.service.removeSafeFood(familyMemberId, foodItem).subscribe();
      
      spectator.expectOne(`/settings/family/${familyMemberId}/safefoods/${foodItem}`, HttpMethod.DELETE);
    });
  });

  describe('addFoodPreference', () => {
    it('makes POST request to add food preference', () => {
      const familyMemberId = '123';
      const foodItem = 'rice';
      const status = 'Include' as const;
      
      spectator.service.addFoodPreference(familyMemberId, foodItem, status).subscribe();
      
      const req = spectator.expectOne(`/settings/family/${familyMemberId}/preferences`, HttpMethod.POST);
      expect(req.request.body).toEqual({ foodItem, status });
    });
  });

  describe('removeFoodPreference', () => {
    it('makes DELETE request to remove food preference', () => {
      const familyMemberId = '123';
      const foodItem = 'rice';
      
      spectator.service.removeFoodPreference(familyMemberId, foodItem).subscribe();
      
      spectator.expectOne(`/settings/family/${familyMemberId}/preferences/${foodItem}`, HttpMethod.DELETE);
    });
  });
}); 