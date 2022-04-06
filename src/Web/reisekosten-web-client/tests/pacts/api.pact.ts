import { Bericht, Reise } from '@/model';
import { InteractionObject, Matchers } from '@pact-foundation/pact';
import { pactWith } from 'jest-pact';
import supertest from 'supertest';

pactWith(
  {
    consumer: 'Reisekosten Web UI',
    provider: 'Reisekosten Backend',
  },
  provider => {
    describe('Erfassen', () => {
      it('akzeptiert Formular', async () => {
        const interaction: InteractionObject = {
          state: 'Buchhaltung ohne Einträge',
          uponReceiving: 'Reisekostenformular mit Daten',
          withRequest: {
            method: 'POST',
            path: '/erfassen',
            headers: {
              'Content-Type': 'application/json',
            },
            body: {
              anfang: Matchers.iso8601DateTimeWithMillis(),
              ende: Matchers.iso8601DateTimeWithMillis(),
              zielort: Matchers.string('Zielort'),
              grund: Matchers.string('Grund'),
            },
          },
          willRespondWith: {
            status: 202,
          },
        };

        await provider.addInteraction(interaction);

        const client = () => {
          const url = provider.mockService.baseUrl;
          return supertest(url);
        };

        await client()
          .post('/erfassen')
          .set('Content-Type', 'application/json')
          .send({
            anfang: new Date(),
            ende: new Date(),
            zielort: 'Zielort',
            grund: 'Grund',
          })
          .expect(202);
      });
    });

    describe('Auswertung', () => {
      describe('ohne eingesendete Formulare', () => {
        it('gibt leere Auswertung zurück', async () => {
          const interaction: InteractionObject = {
            state: 'Buchhaltung ohne Einträge',
            uponReceiving: 'Anforderung zur Auswertung',
            withRequest: {
              method: 'GET',
              path: '/auswertung',
              headers: {
                Accept: 'application/json',
              },
            },
            willRespondWith: {
              status: 200,
              headers: { 'Content-Type': 'application/json' },
              body: {
                summe: 0,
                reisen: [],
              },
            },
          };

          await provider.addInteraction(interaction);

          const client = () => {
            const url = provider.mockService.baseUrl;
            return supertest(url);
          };

          await client()
            .get('/auswertung')
            .set('Accept', 'application/json')
            .expect(200);
        });
      });

      describe('mit 2 eingesendeten Formularen', () => {
        it('gibt Auswertung mit 2 Reisen und Summe zurück', async () => {
          const interaction: InteractionObject = {
            state: 'Buchhaltung mit 2 Einträgen',
            uponReceiving: 'Anforderung zur Auswertung',
            withRequest: {
              method: 'GET',
              path: '/auswertung',
              headers: {
                Accept: 'application/json',
              },
            },
            willRespondWith: {
              status: 200,
              headers: { 'Content-Type': 'application/json' },
              body: {
                summe: Matchers.integer(18),
                reisen: Matchers.like([
                  {
                    anfang: Matchers.iso8601DateTime(),
                    grund: Matchers.string('Grund 1'),
                    zielort: Matchers.string('Zielort 1'),
                    pauschale: 6,
                  },
                  {
                    anfang: Matchers.iso8601DateTime(),
                    grund: Matchers.string('Grund 2'),
                    zielort: Matchers.string('Zielort 2'),
                    pauschale: 12,
                  },
                ]),
              },
            },
          };

          await provider.addInteraction(interaction);

          const client = () => {
            const url = provider.mockService.baseUrl;
            return supertest(url);
          };

          await client()
            .get('/auswertung')
            .set('Accept', 'application/json')
            .expect(200);
        });
      });
    });
  },
);
